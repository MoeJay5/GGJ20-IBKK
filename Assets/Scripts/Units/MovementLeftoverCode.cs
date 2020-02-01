/*
 
    private void Update()
    {
        
        if (movingInitiatedUnit == false)
            return;

        Node startingNode = currentlyInitiatedUnit.GetMyGridNode();
        Node destinationNode = GetMousedOverNode();
        if (GetMousedOverNode() == null)
            return;

        //Will calculate path AND show the path in-game
        Path movementPath = Astar.CalculatePath(startingNode, destinationNode, generatedGrid);

        if (InputListener.Instance.PressedDown_Mouse_LeftClick)
            OrderUnitMovement(currentlyInitiatedUnit, movementPath);
            
    }

    //Helper Functions 

    private Node GetMousedOverNode()
    {
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.GetComponent<Node>();
    }

    private void OrderUnitMovement(Unit unitToMove, Path movementPath)
    {
        movingInitiatedUnit = false;
        StartCoroutine(MoveUnitAlongPath(unitToMove, movementPath));
    }
    private IEnumerator MoveUnitAlongPath(Unit unitToMove, Path movementPath)
    {
        Node prevNode = unitToMove.GetMyGridNode();
        foreach (Node nextNode in movementPath.nodes)
        {
            //Move Unit to Node
            float oneNodeMovementDuration = 2.0f;
            float t = 0;
            float tickDuration = 0.01f;
            while (t < oneNodeMovementDuration)
            {
                Vector3 newPos = Vector3.Lerp(prevNode.transform.position, nextNode.transform.position, t);
                newPos.y = unitToMove.transform.position.y;
                unitToMove.transform.position = newPos;

                t += tickDuration;
                yield return new WaitForSeconds(tickDuration);
            }

            prevNode = nextNode;
            yield return new WaitUntil(() => unitToMove.GetMyGridNode() == nextNode);
        }

        //Done moving along path
        movingInitiatedUnit = true;
    }
*/