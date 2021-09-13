using UnityEngine;

public class EvidenceDictionary : ObjectDictionary<Evidence, EvidenceList>, ICourtRecordObjectDictionary
{
    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
        Evidence altEvidence = CurrentObjectDictionary[evidenceName].AltEvidence;
        SubstituteValueWithAlt(evidenceName, altEvidence);
    }
    
    /// <summary>
    /// Returns the evidence at the specified index as an ICourtRecordObject to be used in the court record menu.
    /// </summary>
    /// <param name="index">The index of the evidence to get.</param>
    /// <returns>The evidence as an ICourtRecordObject</returns>
    public ICourtRecordObject GetObjectAtIndex(int index)
    {
        return this[index];
    }
}
