using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class UIRenderer : MonoBehaviour
    {
        private List<Control> controls = new List<Control>();
        public bool playing;
        
        private void Start()
        {
            playing = true;
            controls.AddRange(GetComponents<Control>());
            controls.AddRange(GetComponentsInChildren<Control>());
            foreach (var control in controls) control.Start();
        }

        private void OnDrawGizmosSelected()
        {
            if (playing) return;
            controls.AddRange(GetComponents<Control>());
            controls.AddRange(GetComponentsInChildren<Control>());
            foreach (var control in controls) control.ViewInEditor();
        }
    }
}