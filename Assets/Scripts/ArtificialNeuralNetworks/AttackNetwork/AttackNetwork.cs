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
    public class AttackNetwork : MonoBehaviour, INeuralNetwork
    {
        public enum DECISION
        {
            ATTACK_CLOSEST,
            ATTACK_WEAKEST,
            ATTACK_WEAKEST_IN_RANGE,
            REST,
            RUN,
        }

        public enum TRAINING_STATE
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

                // Manual assignment of neural network weights
                this.network.SetWeights(new double[]
                {
                    //-0.02666, -0.02978, 0.02464, -0.04696, 0.00475, -0.01867, 0.00817, 0.00581, 0.01431, -0.03758, 0.00035, -0.00202, -0.00122, 0.02057, -0.00035, 0.01881, -0.01804, 0.02608, -0.00083, -0.00172, -0.05193, 0.00547, -0.00524, -0.00638, -0.02777, 0.03782, -0.03390, 0.01865, 0.00624, -0.02150, 0.02801, -0.00447, -0.00271, -0.02967, 0.00092, 0.01248, 0.03602, -0.01324, 0.01907, 0.05790, 0.05408, -0.01376, -0.01238, 0.02659, 0.00684, -0.02187, -0.01537, 0.06595, 0.02665, -0.01481, 0.01995, 0.04246, -0.02204, -0.01515, 0.00851, -0.03026, 0.02468, 0.01596, 0.00142, -0.00288, -0.19915, -0.33981, -0.15879, -0.32984, -0.79322, -0.14426, -0.24615, -0.11502, -0.23892, -0.53643, -0.29382, -0.50136, -0.23428, -0.48664, -1.17355, -0.36018, -0.61458, -0.28719, -0.59654, -1.40447, -0.44061, -0.75179, -0.35129, -0.72971, -1.56440, -0.16512, -0.28174, -0.13165, -0.27347, -0.61331, -0.88257, -1.83879, -0.34757, -1.81380, -5.71495
					0.25667, -0.29755, 0.14610, -0.29884, 0.29459, -0.24761, 0.29150, -0.26196, 0.13577, -0.28946, 0.29019, -0.23096, 0.14974, -0.11808, 0.06485, -0.10660, 0.14095, -0.09185, 0.28250, -0.26949, 0.06953, -0.24641, 0.28460, -0.23532, 0.25556, -0.22995, 0.08756, -0.23323, 0.29608, -0.25044, 0.26102, -0.22603, 0.09702, -0.23975, 0.23777, -0.17716, 0.24387, -0.21169, 0.10793, -0.13128, 0.26443, -0.18374, 0.17651, -0.15193, 0.08781, -0.18979, 0.17786, -0.08668, 0.16831, -0.14870, 0.08068, -0.08348, 0.12288, -0.12962, 0.29184, -0.29803, 0.14614, -0.23592, 0.29126, -0.23182, 0.58366, -0.82351, -0.78786, -0.81794, -0.80492, -1.10174, 0.27749, 0.62862, 0.28981, -0.52474, -0.36525, -0.54551, -0.20880, -0.53043, -1.17895, -1.40571, -0.07469, 0.51266, -0.05124, -1.39288, 0.19129, -1.19626, -0.87935, -1.17795, -1.57602, -0.91077, 0.14990, 0.45552, 0.16213, -0.60249, 1.51111, -2.67891, -2.03680, -2.66491, -5.72802
				});

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

        public static DECISION GetDecision(int decision)
        {
            return (DECISION)decision;
        }

        #endregion

    }
}
