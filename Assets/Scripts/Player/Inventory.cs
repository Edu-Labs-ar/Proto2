using DG.Tweening;
using UnityEngine;
using EduLabs.Mechanics.Interaction;

namespace EduLabs.Player
{

  public class Inventory : MonoBehaviour
  {

    [Header("References")]
    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform upperLeft;
    public Transform upperRight;

    [Header("Settings")]
    [Range(0.5f, 5f)]
    public float pickUpSpeed = 1f;

    private Interactuable _itemLeft;
    public Interactuable itemLeft { get { return _itemLeft; } }
    private Interactuable _itemRight;
    public Interactuable itemRight { get { return _itemRight; } }

    public bool HasSpace()
    {
      return _itemLeft == null || _itemRight == null;
    }

    public bool HasSpace(Hand hand)
    {
      return (hand == Hand.Left) ? (_itemLeft == null) : (_itemRight == null);
    }

    public Interactuable GetItem(Hand hand) {
      return hand == Hand.Left ? _itemLeft : _itemRight;
    }

    public void Grab(Interactuable item, Hand hand, bool bottom)
    {
      Transform pivot = hand == Hand.Left ? (bottom ? bottomLeft : upperLeft) : (bottom ? bottomRight : upperRight);

      item.GetTransform().parent = pivot;
      item.GetTransform().DOMove(pivot.position, 1 / pickUpSpeed).SetEase(Ease.InCubic).OnComplete(() =>
      {
        if (hand == Hand.Left) _itemLeft = item;
        else _itemRight = item;
      });
    }
  }
}