using UnityEngine;

public class EvidencePreloader : IEvidenceController
{
    public void AddEvidence(string evidence)
    {
        Debug.Log($"Loaded {evidence}.");
        Resources.Load(evidence);
    }

    public void RemoveEvidence(string evidence)
    {
    }

    public void AddToCourtRecord(string actor)
    {
        Debug.Log($"Loaded {actor}.");
        Resources.Load(actor);
    }

    public void RequirePresentEvidence()
    {
    }

    public void SubstituteEvidenceWithAlt(string evidence)
    {
    }

    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
    }
}
