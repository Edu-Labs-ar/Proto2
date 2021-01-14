using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EduLabs.Mechanics.Drag
{
  using Animation;
  using Interaction;
  public class GenericDraggable : MonoBehaviour, Draggable
  {

    [Header("References")]
    public MeshRenderer targetMesh;

    [Header("Animation")]
    [Range(0f, .1f)]
    public float baseRimWidth = .005f;

    [Range(0f, .1f)]
    public float tinkleRimWidth = .012f;


    private OutlineAnimator outlineAnimator;

    private Renderer[] renderers;
    private Dictionary<Renderer, Material[]> childMaterials = new Dictionary<Renderer, Material[]>();
    private Dictionary<Renderer, MaterialPropertyBlock> childProperties = new Dictionary<Renderer, MaterialPropertyBlock>();
    private Material[] silhouetteMaterial;


    private float floorDistance;
    private Vector3 groundAnchorScreen;
    private Vector3 initialAnchor;

    private Rigidbody rb;

    void OnValidate()
    {
      if (outlineAnimator != null)
      {
        outlineAnimator.baseWidth = baseRimWidth;
        outlineAnimator.tinkleWidth = tinkleRimWidth;
      }
    }

    void OnEnable()
    {
      Material[] outlineMaterials = new Material[2];
      outlineAnimator = new OutlineAnimator(targetMesh, outlineMaterials);
      outlineAnimator.baseWidth = baseRimWidth;
      outlineAnimator.tinkleWidth = tinkleRimWidth;

      Addressables
          .LoadAssetAsync<Material>(GameInfo.ADDRESSABLE_SILHOUETTE)
          .Completed += handle =>
          {
            silhouetteMaterial = new Material[] { handle.Result };
            outlineMaterials[0] = handle.Result;
          };

      renderers = GetComponentsInChildren<Renderer>();
      rb = GetComponent<Rigidbody>();
    }

    public void Drag(Vector2 delta)
    {
      groundAnchorScreen += (Vector3)delta;
      Vector3 targetPos = UnityEngine.Camera.main.ScreenToWorldPoint(groundAnchorScreen);
      targetPos.y = initialAnchor.y + floorDistance;
      if (rb != null)
        rb.velocity = (targetPos - transform.position) / Time.deltaTime;
      else
        transform.position = targetPos;
    }

    public void Select()
    {
      EnableSilhouettes();
      outlineAnimator.Select();

      RaycastHit hit;
      if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, GameInfo.environmentLayer))
      {
        floorDistance = hit.distance;
        initialAnchor = hit.point;
      }
      else
      {
        floorDistance = 0f;
        initialAnchor = transform.position;
      }
      groundAnchorScreen = UnityEngine.Camera.main.WorldToScreenPoint(initialAnchor);
    }

    public void Unselect() { outlineAnimator.Unselect(DisableSilhouettes); }

    public void Grab() { outlineAnimator.Grab(DisableSilhouettes); }


    private void EnableSilhouettes()
    {
      childMaterials.Clear();
      childProperties.Clear();

      MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
      targetMesh.GetPropertyBlock(propBlock);

      Texture meshTexture = propBlock.GetTexture("_BaseMap");
      if (meshTexture != null)
        propBlock.SetTexture("_BaseMap", Texture2D.whiteTexture);
      MaterialPropertyBlock childProps = new MaterialPropertyBlock();
      foreach (Renderer render in renderers)
      {
        if (render.Equals(targetMesh)) continue;
        render.GetPropertyBlock(childProps);
        childProperties.Add(render, childProps);
        render.SetPropertyBlock(propBlock);

        childMaterials.Add(render, render.materials);
        render.materials = silhouetteMaterial;
      }
      if (meshTexture != null)
        propBlock.SetTexture("_BaseMap", meshTexture);
    }

    private void DisableSilhouettes()
    {
      foreach (KeyValuePair<Renderer, Material[]> entry in childMaterials)
      {
        entry.Key.materials = entry.Value;
        entry.Key.SetPropertyBlock(childProperties[entry.Key]);
      }

      childMaterials.Clear();
      childProperties.Clear();
    }

    public Type GetInteractor() { return typeof(Dragger); }
    public Transform GetTransform() { return transform; }

    public InteractionType GetSupportedInteractions() { return InteractionType.Click; }
  }
}