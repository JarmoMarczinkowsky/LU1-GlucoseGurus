using System;
using UnityEngine;

public class Note
{
    public Guid id;
    public DateTime date;
    public string? text;
    public Guid parentGuardianId;
    public Guid patientId;
}
