using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
   public Light sun;
   public Material skyboxMaterial;
   public float cycleDuration = 30f;
   
   public Transform fireLightsGroup;
   public Transform fireParticlesGroup;

   [Header("Day")]
   public Color dayZenith = new Color(0.53f, 0.81f, 0.98f);
   public Color dayHorizon = new Color(0.73f, 0.89f, 1f);
   public float dayIntensity = 1f;
   
   [Header("Night")]
   public Color nightZenith = new Color(0.01f, 0.01f, 0.07f);
   public Color nightHorizon = new Color(0.01f, 0.01f, 0.05f);
   public float nightIntensity = 0.05f;

   private float timer;
   
   [Range(0f, 1f)]
   public float startTime = 0f;
   
   
   void Start()
   {
       
       skyboxMaterial = Instantiate(skyboxMaterial);
       RenderSettings.skybox = skyboxMaterial;

       timer = startTime * cycleDuration;
       
       skyboxMaterial = Instantiate(skyboxMaterial);
       RenderSettings.skybox = skyboxMaterial;
   }

   void Update()
   {
       timer = (timer + Time.deltaTime) % cycleDuration;

       float angle = (timer / cycleDuration) * 360f - 90f;

       sun.transform.rotation = Quaternion.Euler(angle, 0f, 0f);
       
       float sunHeight = -sun.transform.forward.y;

       // Освещение солнца
       float lightT = Mathf.InverseLerp(0.1f, -0.2f, sunHeight);
       sun.intensity = Mathf.Lerp(dayIntensity, nightIntensity, lightT);

       if (skyboxMaterial != null)
       {
           // Небо начинает темнеть только после захода солнца
           float skyT = Mathf.InverseLerp(-0.05f, -0.4f, sunHeight);
           
           bool isNight = skyT > 0.3f;

           // Все источники света
           foreach (FireFlicker fire in fireLightsGroup.GetComponentsInChildren<FireFlicker>())
           {
               fire.baseIntensity = Mathf.Lerp(0f, 4f, skyT);
           }

           // Все частицы
           foreach (ParticleSystem ps in fireParticlesGroup.GetComponentsInChildren<ParticleSystem>())
           {
               var emission = ps.emission;
               emission.rateOverTime = Mathf.Lerp(0f, 40f, skyT);
           }

           skyboxMaterial.SetColor("_ZenithColor",
               Color.Lerp(dayZenith, nightZenith, skyT));

           skyboxMaterial.SetColor("_HorizonColor",
               Color.Lerp(dayHorizon, nightHorizon, skyT));

           skyboxMaterial.SetFloat("_AtmosphereThickness",
               Mathf.Lerp(0.5f, 1f, skyT));

           skyboxMaterial.SetFloat("_EnableStars", skyT);

           skyboxMaterial.SetFloat("_SkyExposure",
               Mathf.Lerp(1f, 0.15f, skyT));
       }
   }
}
