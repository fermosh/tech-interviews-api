namespace TechnicalInterviewHelper.Model.Entities.Comparers
{
    using System.Collections.Generic;

    /// <summary>
    /// Skill equality comparer
    /// </summary>
    public class SkillComparer : EqualityComparer<Skill>
    {
        /// <summary>
        /// Overrides the defualt Skill Comparer when needed
        /// </summary>
        /// <param name="s1">Skill One</param>
        /// <param name="s2">Skill Two</param>
        /// <returns>Returns a oolean</returns>
        public override bool Equals(Skill s1, Skill s2)
        {
            if (s1 == null && s2 == null)
            {
                return true;
            }
            else if (s1 == null || s2 == null)
            {
                return false;
            }

            return s1.Id == s2.Id;
        }

        /// <summary>
        /// Return a integer hash code
        /// </summary>
        /// <param name="skill">The skill to evaluate</param>
        /// <returns>Integer hash</returns>
        public override int GetHashCode(Skill skill)
        {
            int hash = skill.Id * skill.Name.Length;
            return hash.GetHashCode();
        }
    }
}
