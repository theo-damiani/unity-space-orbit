using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteStars : MonoBehaviour {
 
 
    private Transform tx;
    private ParticleSystem.Particle[] points;
 
    public int starsMax = 100;
    public float starSize = 1;
    public float starMaxDistance = 10;
    public float starMaxSpawnSize = 1;
    public float starClipDistance = 1;
    private float starMaxDistanceSqr;
    private float starClipDistanceSqr;
 
 
    void Start () {
        tx = transform;
        starMaxDistanceSqr = starMaxDistance * starMaxDistance;
        starClipDistanceSqr = starClipDistance * starClipDistance;
    }
 
 
    private void CreateStarsInSphere() {
        points = new ParticleSystem.Particle[starsMax];
 
        for (int i = 0; i < starsMax; i++) {
            points[i].position = Random.insideUnitSphere * starMaxDistance + tx.position;
            points[i].startColor = new Color(1,1,1, 1);
            points[i].startSize = starSize;
        }
    } 
 
    void Update () {

        if( points == null ) 
        {
            CreateStarsInSphere();
        }
 
        for (int i = 0; i < starsMax; i++) {
 
            if((points[i].position - tx.position).sqrMagnitude > starMaxDistanceSqr) 
            {
                points[i].position = Random.insideUnitSphere.normalized * starMaxDistance + tx.position;
            }
 
            if ((points[i].position - tx.position).sqrMagnitude <= starClipDistanceSqr) 
            {
                float percent = (points[i].position - tx.position).sqrMagnitude / starClipDistanceSqr;
                points[i].startColor = new Color(1,1,1, percent);
                points[i].startSize = percent * starSize;
            }
        }
        GetComponent<ParticleSystem>().SetParticles ( points, points.Length );
    }
}
