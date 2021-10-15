using System;
using TMPro;
using UnityEngine;

namespace SixtyMeters.scripts
{
    public class TextDisplay : MonoBehaviour
    {
    
        private TextMeshPro _tm;
        
        private void Awake()
        {
            _tm = GetComponent<TextMeshPro>();
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetText(String text)
        {
            _tm.text = text;
        }
    }
}
