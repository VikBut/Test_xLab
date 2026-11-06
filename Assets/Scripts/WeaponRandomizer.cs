using System.Collections.Generic;
using UnityEngine;

public class WeaponRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public Transform weapon;

        [HideInInspector] public Transform originalParent;
        [HideInInspector] public Vector3 originalLocalPosition;
        [HideInInspector] public Quaternion originalLocalRotation;
        [HideInInspector] public Vector3 originalLocalScale;
    }

    public List<WeaponData> weapons = new();

    void Start()
    {
        foreach (var w in weapons)
        {
            w.originalParent = w.weapon.parent;
            w.originalLocalPosition = w.weapon.localPosition;
            w.originalLocalRotation = w.weapon.localRotation;
            w.originalLocalScale = w.weapon.localScale;
        }
    }

    public void ShuffleWeapons()
    {
        List<Transform> hands = new();

        foreach (var w in weapons)
            hands.Add(w.weapon.parent);

        // Fisher-Yates
        for (int i = hands.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (hands[i], hands[j]) = (hands[j], hands[i]);
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weapon.SetParent(hands[i], false);
            weapons[i].weapon.localPosition = weapons[i].originalLocalPosition;
            weapons[i].weapon.localRotation = weapons[i].originalLocalRotation;
            weapons[i].weapon.localScale = weapons[i].originalLocalScale;
        }
    }

    public void ResetWeapons()
    {
        foreach (var w in weapons)
        {
            w.weapon.SetParent(w.originalParent, false);
            w.weapon.localPosition = w.originalLocalPosition;
            w.weapon.localRotation = w.originalLocalRotation;
            w.weapon.localScale = w.originalLocalScale;
        }
    }
}