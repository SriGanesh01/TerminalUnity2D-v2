using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace username
{
    public class GetUsername : MonoBehaviour
    {
        [SerializeField] private Canvas canvasLogin;
        [SerializeField] private GameObject canvasTerminal;
        public Terminal.Terminalv3 terminal;
        public string username;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnSubmit()
        {
            username = GameObject.Find("InputField").GetComponent<UnityEngine.UI.InputField>().text;
            terminal.name = username;
            canvasLogin.enabled = false;
            canvasTerminal.SetActive(true);
        }
    }

}