using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class FullScreenAnimation
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        
        [UnityTest]
        public IEnumerator FullScreenAnimationsCannotBeSkipped()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/FullScreenAnimation - Test Scene.unity", new LoadSceneParameters());
            var speechPanel = TestTools.FindInactiveInSceneByName<GameObject>("SpeechPanel");
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayer>();
            yield return null; 
           
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return TestTools.WaitForState(() => !narrativeScriptPlayer.Waiting);
            Assert.IsTrue(speechPanel.activeInHierarchy);
        }
    }
}

