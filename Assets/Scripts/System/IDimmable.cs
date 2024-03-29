public interface IDimmable
{
    public float OriginalDimmableValue { get; set; }
    public void AnnounceEnable();
    public void Dim(float dimPercentage);
    public float CurrentDim();
    public void AnnounceDisable();
}
