﻿namespace TechnicalInterviewHelper.Model
{
    /// <summary>
    /// To get information about a position.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Gets or sets the competency identifier.
        /// </summary>
        /// <value>
        /// The competency identifier.
        /// </value>
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        public int LevelId { get; set; }

        /// <summary>
        /// Gets or sets the domain identifier.
        /// </summary>
        /// <value>
        /// The domain identifier.
        /// </value>
        public int DomainId { get; set; }
    }
}