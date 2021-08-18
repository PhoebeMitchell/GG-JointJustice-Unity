using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuNavigatorTests : IPrebuildSetup
{
    private MockHighlightableMenuItem[] _mockHighlightableMenuItems;
    private MenuNavigator _menuNavigator;
    
    public void Setup()
    {
        _mockHighlightableMenuItems = new MockHighlightableMenuItem[]
        {
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem()
        };
        _menuNavigator = new MenuNavigator(3, _mockHighlightableMenuItems.ToArray<IHightlightableMenuItem>());
        Debug.Log(_menuNavigator == null);
    }
    
    [Test, Order(0)]
    public void Increment50()
    {
        Debug.Log(_menuNavigator == null);
        IncrementTest(50);
        Debug.Log(_menuNavigator.CurrentIndex);
        Assert.AreEqual(_menuNavigator.CurrentIndex, 4);
    }

    [Test, Order(1)]
    public void Decrement3()
    {
        DecrementTest(3);
        Assert.AreEqual(_menuNavigator.CurrentIndex, 1);
    }
    
    [Test, Order(2)]
    public void Decrement20()
    {
        DecrementTest(20);
        Assert.AreEqual(_menuNavigator.CurrentIndex, 0);
    }
    
    [Test, Order(3)]
    public void CheckIncrements()
    {
        for (int i = 0; i < _menuNavigator.IndexCount; i++)
        {
            _menuNavigator.IncrementPosition();
            Assert.AreEqual(_menuNavigator.CurrentIndex, i);
        }
    }
    
    [Test, Order(4)]
    public void CheckDecrements()
    {
        for (int i = _menuNavigator.IndexCount - 1; i >= 0; i--)
        {
            _menuNavigator.DecrementPosition();
            Assert.AreEqual(_menuNavigator.CurrentIndex, i);
        }
    }
    
    [Test, Order(5)]
    public void Increment2()
    {
        IncrementTest(2);
        Assert.AreEqual(_menuNavigator.CurrentIndex, 2);
    }

    private void IncrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.IncrementPosition();
        }
    }

    private void DecrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.DecrementPosition();
        }
    }
}
