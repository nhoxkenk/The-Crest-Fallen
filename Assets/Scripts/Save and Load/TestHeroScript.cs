using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeroScript : MonoBehaviour, IBind<PlayerData>
{
    [field: SerializeField] public string Id { get; set; } = "Test Id";
    [SerializeField] private PlayerData data;

    public void Bind(PlayerData data)
    {
        Debug.Log("Binded");
        this.data = data;
        this.data.Id = Id;
        transform.position = data.position;
        transform.rotation = data.rotation;
    }

    private void Update()
    {
        data.position = transform.position;
        data.rotation = transform.rotation;
    }
}
