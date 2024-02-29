namespace FirstTrainingProject
{
    /// <summary>
    /// Enum to specify the direction of cell creation
    /// </summary>
    public enum CellDirection
    {
        /// <summary>
        /// Left - default direction, no rotation needed
        /// </summary>
        Left = 0,

        /// <summary>
        /// Top - need a 90 degree rotation
        /// </summary>
        Top = 90,

        /// <summary>
        /// Right - need a 180 degree rotation
        /// </summary>
        Right = 180,

        /// <summary>
        /// Bottom - need a 270 degree rotation
        /// </summary>
        Bottom = 270
    }
}
