using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetworks.Training
{
    public interface INetworkTrainer
    {
        void DisplaySensors();
        void HideSensors();
        void DisplayOutputs();
        void HideOutputs();
        void CheckTrainingInput();
    }
}
