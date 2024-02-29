namespace FirstTrainingProject
{
    /// <summary>
    /// Enum to specify cell type
    /// </summary>
    public enum CellType
    {
        Empty = 0,
        Marked = 1,
        Room = 2, // m.b. removed
        Door = 3,

        /// <summary>
        /// Start cell
        /// </summary>
        Start = 11,

        /// <summary>
        /// Action cell
        /// </summary>
        Action = 12,

        /// <summary>
        /// Dead cell
        /// </summary>
        Dead = 13,

        /// <summary>
        /// End cell
        /// </summary>
        End = 14,

        /// <summary>
        /// Corner cell
        /// </summary>
        Corner = 21,

        /// <summary>
        /// Straight cell
        /// </summary>
        Straight = 22,

        /// <summary>
        /// Branch cell
        /// </summary>
        Branch = 31,

        /// <summary>
        /// Cross cell
        /// </summary>
        Cross = 41
    }
}
