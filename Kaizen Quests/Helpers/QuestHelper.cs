using Kaizen_Quests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaizen_Quests.Helpers
{
    internal static class QuestHelper
    {
        public static List<Quest> CloneQuestListDeep(List<Quest> quests)
        {
            return quests.Select(q => new Quest
            {
                Id = q.Id,
                Title = q.Title,
                Color = q.Color,
                Order = q.Order,
                IsExpanded = q.IsExpanded,
                Goals = q.Goals.Select(g => new Goal
                {
                    Id = g.Id,
                    QuestId = g.QuestId,
                    Text = g.Text,
                    Order = g.Order,
                    IsCompleted = g.IsCompleted,
                    IsAddGoal = g.IsAddGoal
                }).ToList()
            }).ToList();
        }
    }
}
