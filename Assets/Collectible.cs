using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool visible = true;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, 180 * Time.deltaTime);
    }

    public void callingVisibleFunc(bool vis, float time)
    {
        StartCoroutine(setVisibleFunc(vis, time));
    }

    IEnumerator setVisibleFunc(bool vis, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        setVisible(vis);
    }

    public void setVisible(bool vis)
    {
        mesh.enabled = vis;
        visible = vis;
    }
}
