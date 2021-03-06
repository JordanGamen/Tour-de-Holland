using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour
{
    public enum BoardSpaceTypes { SAFE, START, PROPERTY, TRAIN, JAILVISIT, LUCKY, GOTOJAIL}

    [SerializeField] private BoardSpaceTypes boardSpaceType = BoardSpaceTypes.PROPERTY;
    public BoardSpaceTypes BoardSpaceType { get { return boardSpaceType; } }
    [SerializeField] private int boardIndex = 0;
    public int BoardIndex { get { return boardIndex; } set { boardIndex = value; } }
    [SerializeField] private PropertyCard propertyCardOnSpace;
    public PropertyCard PropertyCardOnSpace { get { return propertyCardOnSpace; } }
    [SerializeField] private BoardSpace spaceToTransportTo;
    public BoardSpace SpaceToTransportTo { get { return spaceToTransportTo; } }
}
