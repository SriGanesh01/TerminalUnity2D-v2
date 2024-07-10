using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace initsetup
{
    public class InitialSetup : MonoBehaviour
    {
        [SerializeField] private Canvas canvasLogin;
        [SerializeField] private GameObject canvasTerminal;
        // Start is called before the first frame update
        void Start()
        {
            canvasLogin.enabled = true;
            canvasTerminal.SetActive(false);
        }
    }
}
