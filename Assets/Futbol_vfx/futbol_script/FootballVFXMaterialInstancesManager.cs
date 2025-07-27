using UnityEngine;
public enum MaterialType
{
    ParticleBigFlame,
    ParticleSmallFlame,
    ParticleSpheres,
    TrailLong,
    TrailLightning
}

public class FootballVFXMaterialInstancesManager : MonoBehaviour
{
    [Header("Particle Big Flame")]
    public Material particleMaterialBigFlame; 
    public Color _color1 = Color.white;
    public Color _color2 = Color.white;
    public float _animSpeed = 15.0f;
    public float _noiseScale = 300.0f;
    [Header("Particle Small Flame")]
    public Material particleMaterialSmallFlame;
    public Color _color3 = Color.white;
    public Color _color4 = Color.white;
    public float _animSpeed2 = 2.0f;
    public float _dissolveScale = 30.0f;
    [Header("Particle Spheres")]
    public Material particleMaterialSpheres;
    public Color _color5 = Color.white;
    [Header("Long Trail")]
    public Material trailMaterialLongTrail;
    public Color _color6 = Color.white;
    public Color _color7 = Color.white;
    public float _animSpeed3 = 30.0f;
    public float _dissolveScale2 = 30.0f;
    [Header("Lightning Trail")]
    public Material trailMaterialLightningTrail;
    public Color _color8 = Color.white;
    public Color _color9 = Color.white;
    public float speedX = 1.0f;
    public float speedY = 0.0f;
    
    public bool canChangeMaterial;
    
    void Start() {
        if (particleMaterialBigFlame == null) {
            Debug.LogWarning("No particle material assigned to particleMaterialBigFlame");
            return;
        }
        if (particleMaterialSmallFlame == null) {
            Debug.LogWarning("No single particle material assigned to particleMaterialSmallFlame");
            return;
        }
        if(particleMaterialSpheres == null) {
            Debug.LogWarning("No particle material assigned to particleMaterialSpheres");
            return;
        }
        if (trailMaterialLongTrail == null) {
            Debug.LogWarning("No trail renderer assigned to particleMaterialLongTrail");
            return;
        }
        if (trailMaterialLightningTrail == null) {
            Debug.LogWarning("No trail renderer assigned to particleMaterialLightningTrail");
            return;
        }
        
        SetMaterials();
    }
    
    void Update() {
        
        
        
        if (!canChangeMaterial) {
           return;
        }
        SetMaterials();
    }

    public void ChangeMaterial(MaterialType type) {
        switch (type)
        {
            case MaterialType.ParticleBigFlame:
                if (particleMaterialBigFlame != null)
                {
                    particleMaterialBigFlame.SetColor("_Color", _color1);
                    particleMaterialBigFlame.SetColor("_Color2", _color2);
                    particleMaterialBigFlame.SetFloat("_AnimSpeed", _animSpeed);
                    particleMaterialBigFlame.SetFloat("_NoiseScale", _noiseScale);
                }
                break;

            case MaterialType.ParticleSmallFlame:
                if (particleMaterialSmallFlame != null)
                {
                    particleMaterialSmallFlame.SetColor("_Color_1", _color3);
                    particleMaterialSmallFlame.SetColor("_Color_2", _color4);
                    particleMaterialSmallFlame.SetFloat("_Speed", _animSpeed2);
                    particleMaterialSmallFlame.SetFloat("_DissolveScale", _dissolveScale);
                }
                break;

            case MaterialType.ParticleSpheres:
                if (particleMaterialSpheres != null)
                {
                    particleMaterialSpheres.SetColor("_Color", _color5);
                }
                break;

            case MaterialType.TrailLong:
                if (trailMaterialLongTrail != null)
                {
                    trailMaterialLongTrail.SetColor("_Color_1", _color6);
                    trailMaterialLongTrail.SetColor("_Color_2", _color7);
                    trailMaterialLongTrail.SetFloat("_Speed", _animSpeed3);
                    trailMaterialLongTrail.SetFloat("_DissolveScale", _dissolveScale2);
                }
                break;

            case MaterialType.TrailLightning:
                if (trailMaterialLightningTrail != null)
                {
                    trailMaterialLightningTrail.SetColor("_Color", _color8);
                    trailMaterialLightningTrail.SetColor("_Color2", _color9);
                    trailMaterialLightningTrail.SetFloat("_SpeedX", speedX);
                    trailMaterialLightningTrail.SetFloat("_SpeedY", speedY);
                }
                break;
        }
    }

    public void SetMaterials() {
        // Big flame material setup
        particleMaterialBigFlame.SetColor("_Color", _color1);
        particleMaterialBigFlame.SetColor("_Color2", _color2);
        particleMaterialBigFlame.SetFloat("_AnimSpeed", _animSpeed);
        particleMaterialBigFlame.SetFloat("_NoiseScale", _noiseScale);
        // Small flame material setup
        particleMaterialSmallFlame.SetColor("_Color_1", _color3);
        particleMaterialSmallFlame.SetColor("_Color_2", _color4);
        particleMaterialSmallFlame.SetFloat("_Speed", _animSpeed2);
        particleMaterialSmallFlame.SetFloat("_DissolveScale", _dissolveScale);
        // Spheres material setup
        particleMaterialSpheres.SetColor("_Color", _color5);
        // Long trail material setup
        trailMaterialLongTrail.SetColor("_Color_1", _color6);
        trailMaterialLongTrail.SetColor("_Color_2", _color7);
        trailMaterialLongTrail.SetFloat("_Speed", _animSpeed3);
        trailMaterialLongTrail.SetFloat("_DissolveScale", _dissolveScale2);
        // Lightning trail material setup
        trailMaterialLightningTrail.SetColor("_Color", _color8);
        trailMaterialLightningTrail.SetColor("_Color2", _color9);
        trailMaterialLightningTrail.SetFloat("_SpeedX", speedX);
        trailMaterialLightningTrail.SetFloat("_SpeedY", speedY);
            
        canChangeMaterial = false;
    }
}
