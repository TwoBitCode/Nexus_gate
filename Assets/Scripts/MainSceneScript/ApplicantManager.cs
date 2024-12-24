using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class ApplicantManager:MonoBehaviour
{
    private DayData currentDayData;
    private List<Applicant> applicants = new List<Applicant>();
    private int currentApplicantIndex = -1; // Start with no applicant selected
    private List<OriginSymbolPair> shuffledPairs;

    // Set the DayData for this manager
    public void SetDayData(DayData dayData)
    {
        if (dayData == null)
        {
            Debug.LogError("SetDayData received a null DayData!");
            return;
        }

        if (dayData.originSymbolPairs == null || dayData.originSymbolPairs.Length == 0)
        {
            Debug.LogError("DayData does not contain any origin-symbol pairs!");
            return;
        }

        currentDayData = dayData;

        // Shuffle the origin-symbol pairs
        shuffledPairs = new List<OriginSymbolPair>(dayData.originSymbolPairs);
        shuffledPairs = shuffledPairs.OrderBy(a => Random.value).ToList();

        // Generate applicants based on maxApplicantsPerDay from DayData
        applicants.Clear();
        int applicantCount = dayData.maxApplicantsPerDay; // Respect maxApplicantsPerDay
        for (int i = 0; i < applicantCount; i++)
        {
            applicants.Add(GenerateApplicant());
        }

        currentApplicantIndex = -1; // Reset to start
        Debug.Log($"Generated {applicants.Count} applicants for the day.");
    }


    public List<OriginSymbolPair> GetShuffledPairs()
    {
        if (shuffledPairs == null || shuffledPairs.Count == 0)
        {
            Debug.LogError("Shuffled pairs are not initialized!");
        }

        return shuffledPairs;
    }

    public void GenerateApplicants(DayData dayData)
    {
        if (dayData == null)
        {
            Debug.LogError("DayData is null!");
            return;
        }

        int maxApplicants = dayData.maxApplicantsPerDay;
        Debug.Log($"DayData maxApplicantsPerDay: {maxApplicants}");

        for (int i = 0; i < maxApplicants; i++)
        {
            Applicant applicant = GenerateApplicant();
            if (applicant != null)
            {
                applicants.Add(applicant);
                Debug.Log($"Generated Applicant: {applicant.Name}, Origin: {applicant.Origin}");
            }
        }

        Debug.Log($"Generated {applicants.Count} applicants for the day.");
    }



    public bool GenerateNextApplicant()
    {
        if (currentApplicantIndex + 1 >= applicants.Count)
        {
            Debug.Log("No more applicants to process.");
            return false; // No more applicants
        }

        currentApplicantIndex++;
        Debug.Log($"Moved to applicant {currentApplicantIndex + 1}/{applicants.Count}.");
        return true; // Successfully moved to next applicant
    }


    public Applicant GetCurrentApplicant()
    {
        if (currentApplicantIndex >= 0 && currentApplicantIndex < applicants.Count)
        {
            return applicants[currentApplicantIndex];
        }

        Debug.LogError("No current applicant available!");
        return null;
    }
    public Applicant GenerateApplicant()
    {
        if (currentDayData == null)
        {
            Debug.LogError("DayData is not set in ApplicantManager!");
            return null;
        }

        if (currentDayData.originSymbolPairs == null || currentDayData.originSymbolPairs.Length == 0)
        {
            Debug.LogError("OriginSymbolPairs in DayData are missing!");
            return null;
        }

        // Use shuffled pairs to assign origin and region symbol
        OriginSymbolPair pair = shuffledPairs[Random.Range(0, shuffledPairs.Count)];

        // Generate an applicant
        var applicant = new Applicant
        {
            Name = currentDayData.alienNames[Random.Range(0, currentDayData.alienNames.Length)],
            Origin = pair.origin,
            RegionSymbol = pair.symbol,
            FaceImage = currentDayData.alienImages[Random.Range(0, currentDayData.alienImages.Length)],
            BirthYear = Random.Range(4015, 4065),
            ExpirationYear = Random.Range(4065, 4075),
            RandomDialogues = currentDayData.dialogueLines
        };
        // Decide if the applicant should be invalid
        bool isInvalidApplicant = Random.value < 0.3f; // 30% chance of being invalid
        if (isInvalidApplicant)
        {
            GenerateInvalidData(applicant); // Modify applicant to be invalid
        }

        Debug.Log($"Generated Applicant: {applicant.Name}, Origin: {applicant.Origin}, Valid: {!isInvalidApplicant}");
        return applicant;
    }


public bool AreAllApplicantsProcessed()
    {
        return currentApplicantIndex >= applicants.Count - 1;
    }


    private void GenerateInvalidData(Applicant applicant)
    {
        int invalidType = Random.Range(0, 3); // 0: Invalid Birth Year, 1: Expired Passport, 2: Mismatched Symbol

        switch (invalidType)
        {
            case 0: // Invalid Birth Year
                applicant.BirthYear = Random.Range(6080, 7000); // Unrealistic year
                Debug.Log("Invalid applicant: Unrealistic birth year.");
                break;

            case 1: // Expired Passport
                int currentYear = 4065;
                applicant.ExpirationYear = Random.Range(currentYear - 10, currentYear - 1); // Expired year
                Debug.Log("Invalid applicant: Expired passport.");
                break;

            case 2: // Mismatched Origin-Symbol Pair
                applicant.RegionSymbol = currentDayData.alienImages[Random.Range(0, currentDayData.alienImages.Length)]; // Random symbol
                Debug.Log("Invalid applicant: Origin and symbol mismatch.");
                break;
        }
    }

}
