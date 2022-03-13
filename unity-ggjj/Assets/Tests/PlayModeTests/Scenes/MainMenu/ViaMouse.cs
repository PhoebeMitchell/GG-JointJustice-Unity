﻿using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.MainMenu
{
    public class ViaMouse : InputTest
    {
        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanEnterAndCloseTwoSubMenusIndividually()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = TestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");
            
            RectTransform openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            RectTransform openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            RectTransform closeSecondSubMenuButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");
            RectTransform closeFirstSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return SetMousePosition(openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return SetMousePosition(openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            yield return SetMousePosition(closeSecondSubMenuButton.position + closeSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return SetMousePosition(closeFirstSubMenuButton.position + closeFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanEnterAndCloseTwoSubMenusWithCloseAllButton()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = TestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");
            
            RectTransform openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            RectTransform openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            RectTransform closeAllSubMenusButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return SetMousePosition(openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return SetMousePosition(openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            yield return SetMousePosition(closeAllSubMenusButton.position + closeAllSubMenusButton.localScale * 0.5f);
            yield return PressForFrame(Mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(secondSubMenu.Active);
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanStartGame()
        {
            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = TestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            
            RectTransform startGameButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "NewGameButton");

            yield return SetMousePosition(startGameButton.position + startGameButton.localScale * 0.5f);
            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));
            yield return PressForFrame(Mouse.leftButton);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));

            SceneManagerAPI.overrideAPI = null;
        }
    }
}