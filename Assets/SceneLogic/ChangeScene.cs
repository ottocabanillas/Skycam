using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConfigurationUtilities;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
  [SerializeField]
  Button beginProgramButton;

  void Start()
  {
    beginProgramButton.onClick.AddListener(OnBeginProgramButtonOnClicked);
  }

  private void OnBeginProgramButtonOnClicked()
  {
    if (ConfigurationUtilities.AreRequiredConfigValuesSet() && !ConfigurationUtilities.IsUserChangingConfig())
    {
      // Carga la escena donde preguntamos si quiere cambiar de configuracion o no
      SceneManager.LoadScene(3);
    }
    else 
    {
      // Carga la escena de configuracion inicial
      SceneManager.LoadScene(1);
    }
  }
}
