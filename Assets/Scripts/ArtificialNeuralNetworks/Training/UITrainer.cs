using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetworks.Training
{
    using ArtificialNeuralNetworks.AttackNetwork;

    public class UITrainer : MonoBehaviour, INetworkTrainer
    {
        private Trainer trainer;
        private bool sensorsVisible, outputsVisible;
        private IDictionary<string, double> currentSensorValues;
        private string[] outputs = new string[] { "Attack Closest", "Attack Weakest", "Attack Weakest in Range", "Rest", "Run" };

        #region GUI properties
        private int outputWidth = 300;
        private int outputHeight = 100;
        #endregion

        private bool showSelectionSaved = false;
        private double[] lastSelection;

        public void SetTrainer(Trainer trainer)
        {
            this.trainer = trainer;
        }

        public void DisplaySensors() 
        {
            this.currentSensorValues = this.trainer.GetSensorData();
            this.sensorsVisible = true;
            Debug.Log("Displaying sensors");
        }

        public void HideSensors() 
        {
            this.currentSensorValues = null;
            this.sensorsVisible = false;
            Debug.Log("Hiding sensors");
        }

        public void DisplayOutputs() 
        {
            this.outputsVisible = true;
            Debug.Log("Displaying output choices");
        }

        public void HideOutputs() 
        {
            this.outputsVisible = false;
            Debug.Log("Hiding output choices");
        }

        public void CheckTrainingInput()
        {
            if (!this.showSelectionSaved)
            {
                for (var i = 0; i < outputs.Length; i++)
                {
                    if (Input.GetKeyDown(KeyCode.F1 + i))
                    {
                        var selection = new double[outputs.Length];
                        selection[i] = 1;
                        this.showSelectionSaved = true;
                        this.selectOutput(selection);
                        break;

                    }
                }
            }
        }

        void OnGUI()
        {
            if (!this.showSelectionSaved)
            {
                if (this.outputsVisible)
                    this.showOutputs();
                if (this.sensorsVisible)
                    this.showSensors();
            }
            else
                StartCoroutine(this.showSelection());
        }

        private void showSensors()
        {
            int maxCols = 2, currentRow = 0;
            StringBuilder text = new StringBuilder();
            text.AppendLine("Active character (" + this.trainer.characterName + ") inputs:\r\n");
            GUIStyle style = GUI.skin.button;
            style.alignment = TextAnchor.UpperLeft;

            // Calculate text for display
            if (this.currentSensorValues != null)
            {
                foreach (var key in this.currentSensorValues.Keys)
                { 
                    double d = double.MinValue;
                    currentSensorValues.TryGetValue(key, out d);
                    text.AppendLine(string.Format("{0}: {1}", key, (100f*d).ToString("F2") + " %"));
                }

                text.AppendLine("\r\nPress (F-key) to log decision.");

                // Display values
                GUI.Label(new Rect(0, 0, this.outputWidth, 250), text.ToString(), style);
            }
        }

        private void showOutputs()
        {
            int offset = 250;
            GUIStyle style = GUI.skin.button;
            style.alignment = TextAnchor.UpperLeft;

            for (int i = 0; i < outputs.Length; i++)
                GUI.Label(new Rect(0, offset + (i * 20), this.outputWidth, 20), 
                    string.Format("{0} (F{1})", outputs[i], i+1), style);
        }

        private void selectOutput(double[] outputs)
        {
            this.lastSelection = outputs;
            this.trainer.SetCurrentDecision(outputs);
            this.trainer.PrintTraining();
        }

        private IEnumerator showSelection()
        {
            GUIStyle btnStyle = GUI.skin.button;
            btnStyle.alignment = TextAnchor.MiddleCenter;
            btnStyle.wordWrap = true;
            var vertOffset = this.outputHeight/2f;
            var horizOffset = this.outputWidth/2f;

            var selection = outputs[lastSelection.ToList().IndexOf(1)];

            GUI.Label(new Rect(Screen.width / 2f - horizOffset, Screen.height / 2f - vertOffset, this.outputWidth, this.outputHeight),
                string.Format("Chioce '{0}' saved for {1}", selection, trainer.characterName), 
                btnStyle);

            yield return new WaitForSeconds(1f);

            this.showSelectionSaved = false;

            yield return new WaitForSeconds(0);
        }

    }
}
