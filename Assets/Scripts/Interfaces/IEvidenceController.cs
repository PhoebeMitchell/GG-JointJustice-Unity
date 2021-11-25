using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvidenceController
{
    void AddEvidence(string evidence);
    void RemoveEvidence(string evidence);
    void AddToCourtRecord(string actor);
    void RequirePresentEvidence();
    void SubstituteEvidence(string originalEvidenceName, string newEvidenceName);
    void OnPresentEvidence(ICourtRecordObject evidence);
}
