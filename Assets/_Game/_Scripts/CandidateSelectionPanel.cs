using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandidateSelectionPanel : MonoBehaviour
{
    public List<Candidates> candidates;
}

[System.Serializable]
public class Candidates
{
    public string imageUrl;
    public string id;
    public string partySymbol;
}

/*{
    "candidates":
[
 {
        "imageUrl": null,
  "id": null,
  "partySymbol": null
}
,{
        "imageUrl": null,
  "id": null,
  "partySymbol": null
}
]
  
}*/