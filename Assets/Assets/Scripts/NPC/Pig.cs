using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4); // ���, Ǯ���, �θ���, �ȱ�

        if (_random == 0)
            Wait();
        if (_random == 1)
            Eat();
        if (_random == 2)
            Peek();
        if (_random == 3)
            TryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
    private void Eat()
    {
        anim.SetTrigger("Eat");
        currentTime = waitTime;
        Debug.Log("Ǯ���");
    }
    private void Peek()
    {
        anim.SetTrigger("Peek");
        currentTime = waitTime;
        Debug.Log("�θ���");
    }
}
