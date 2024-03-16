public interface IDimmable
{
    public float OriginalDim { get; set; }
    public void AnnounceEnable();
    public void Dim(float dimPercentage);
    public float CurrentDim();
    public void AnnounceDisable();
}
