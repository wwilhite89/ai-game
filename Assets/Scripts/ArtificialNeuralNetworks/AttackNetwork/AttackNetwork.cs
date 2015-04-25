using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ArtificialNeuralNetworks.Training;
using ArtificialNeuralNetworks.AttackNetwork.Inputs;
using ArtificialNeuralNetworks.Core;

namespace ArtificialNeuralNetworks.AttackNetwork
{
    class AttackNetwork : MonoBehaviour, INeuralNetwork
    {
        public enum DECISION
        {
            ATTACK_CLOSEST,
            ATTACK_WEAKEST,
            ATTACK_WEAKEST_IN_RANGE,
            REST,
            RUN,
        }

        internal enum TRAINING_STATE
        {
            TRAIN,
            LOAD,
            EXECUTE,
            LOAN_AND_EXECUTE,
        }

        public string WorkingDirectory = "AttackNetwork";
        public string OutputPathFormat = "Training_Character_{0}.txt";
        public TRAINING_STATE Mode = TRAINING_STATE.TRAIN;
        public bool OnlyLoadCharacterData = false;

        // ANN Controls
        public double LearnRate = 0.2;
        public double Momentum = 0.2;
        public int MaxEpochs = 10000;
        public double ErrorThreshold = 0.1;

        private CharacterController player;

        private Trainer trainer;
        private TRAINING_STATE mode;
        private BackPropNeuralNet network;
        private List<InputComponent> inputs = new List<InputComponent>();
        private int inputNodes { get { return this.inputs.Count; }}
        private readonly int hiddenNodes = 6, outputNodes = 5;

        private bool trainingStarted = false;
        private bool generated = false;
        private bool attached = false;
        private bool loadedData = false;

        private double[] currentInputs = null;

        void Start()
        {
            this.mode = this.Mode;
            this.player = this.GetComponent<CharacterController>();

            if (player == null)
                throw new UnityException("Character controller null");
            
            // Health Inputs
            inputs.Add(new PlayerHealthInput(player, "Player Health"));

            inputs.Add(new ImmediateTeamHealthInput(player, "Immediate Team Health"));
            inputs.Add(new ImmediateEnemyHealthInput(player, "Immediate Enemy Health"));

            inputs.Add(new GeneralTeamHealthInput(player, "Total Team Health"));
            inputs.Add(new GeneralEnemyHealthInput(player, "Total Enemy Health"));

            inputs.Add(new ImmediatePlayerPowerInput(player, "Immediate Player Power"));
            inputs.Add(new ImmediateTeamPowerInput(player, "Immediate Team Power"));

            inputs.Add(new GeneralPlayerPowerInput(player, "Total Player Power"));
            inputs.Add(new GeneralTeamPowerInput(player, "Total Team Power"));

        }

        void Update()
        {
            if (this.mode != this.Mode)
                Debug.LogWarning("Cannot change the mode of the Attack Network in the middle of a scene.");

            switch (this.mode)
            { 
                case TRAINING_STATE.TRAIN:
                    this.startTraining();
                    break;
                case TRAINING_STATE.LOAD:
                    this.createNetwork();
                    this.loadNetworkData();
                    break;
                case TRAINING_STATE.LOAN_AND_EXECUTE:
                    this.createNetwork();
                    this.loadNetworkData();
                    this.attachNetworkToController();
                    break;
                case TRAINING_STATE.EXECUTE:
                    this.createNetwork();
                    this.attachNetworkToController();
                    break;
                default:
                    break;
            }

        }

        void OnApplicationQuit()
        {
            if (this.trainer != null)
            {
                this.trainer.StopTraining();
                this.trainer.Dispose();
            }
        }

        private void createNetwork()
        {
            if (!this.generated && this.player.IsInitialized())
            {
                this.network = new BackPropNeuralNet(this.inputNodes, this.hiddenNodes, this.outputNodes, true)
                {
                    MaxEpochs = this.MaxEpochs,
                    Momentum = this.Momentum,
                    ErrorThreshold = this.ErrorThreshold,
                    LearnRate = this.LearnRate
                };

                this.generated = true;
            }
        }

        #region Training



        private void startTraining()
        {
            if (!trainingStarted && player.IsInitialized())
            {
                this.startTrainer();
                trainingStarted = true;
            }
        }

        private void startTrainer()
        {
            // Create training
            this.trainer = new Trainer(string.Format(WorkingDirectory + "\\" + OutputPathFormat, player.GetCharacterName()), player.GetCharacterName());

            // Register components
            this.inputs.ForEach(x => this.trainer.RegisterComponent(x));

            // Start training
            this.trainer.StartTraining();

            // Add UI
            var gameTrainer = this.player.gameObject.AddComponent<UITrainer>();
            gameTrainer.SetTrainer(this.trainer);

            // Attach to player
            this.player.AttachTrainer(gameTrainer);
        }

        #endregion

        #region Load Data

        private void loadNetworkData()
        {
            if (!this.loadedData && this.player.IsInitialized())
            {
                this.loadedData = true;
                var files = new System.IO.DirectoryInfo(this.WorkingDirectory).GetFiles();
                var readFiles = files.Where(x => !this.OnlyLoadCharacterData || x.Name == string.Format(this.OutputPathFormat, player.GetCharacterName()));

                foreach (var file in readFiles)
                {
                    Debug.Log(string.Format("[{0}] Attack Network -> reading training file [{1}].", player.GetCharacterName(), file.Name));

                    var count = 0;
                    var lineCount = System.IO.File.ReadAllLines(this.WorkingDirectory + "\\" + file.Name).Where(x => !string.IsNullOrEmpty(x)).Count();
                    var inputValues = new double[lineCount][];
                    var outputValues = new double[lineCount][];

                    // Read training data
                    using (var reader = new System.IO.StreamReader(this.WorkingDirectory + "\\" + file.Name))
                    {
                        while (reader.Peek() > -1)
                        {
                            var data = reader.ReadLine();

                            if (!string.IsNullOrEmpty(data))
                            {
                                var values = data.Trim().Split(' ');
                                var input = values.Take(this.inputNodes);
                                var output = values.Skip(this.inputNodes).Take(this.outputNodes);
                                inputValues[count] = input.Select(x => double.Parse(x)).ToArray();
                                outputValues[count] = output.Select(x => double.Parse(x)).ToArray();
                                count++;
                            }
                        }
                    }

                    // Train Network
                    this.network.Train(inputValues, outputValues, true);
                    Debug.Log(string.Format("[{0}] Attack Network -> Training file '{1}' loaded. {2} lines processed. Current weights...", player.GetCharacterName(), file, lineCount));
                    NeuralNetHelpers.ShowVector(this.network.GetWeights(), 5, 9999, false);
                }
            }
        }

        #endregion

        #region Execute

        private void attachNetworkToController()
        {
            if (!this.attached && this.player.IsInitialized())
            {
                this.player.AttachNetwork(this);
                this.attached = true;
            }
        }

        #endregion

        #region INeuralNetwork Implementation

        public void Sense()
        {
            int i = 0;
            double[] inputs = new double[this.inputNodes];
            this.inputs.ForEach(x => inputs[i++] = x.GetCurrentTraining());
            this.currentInputs = inputs;
        }

        public int Think()
        {
            var outputs = this.network.ComputeOutputs(this.currentInputs).ToList();
            return outputs.FindIndex(x => x == outputs.Max());
        }

        public DECISION GetDecision(int decision)
        {
            return (DECISION)decision;
        }

        #endregion

    }
}
