using SQLite;
using Kaizen_Quests.Models;

namespace Kaizen_Quests.Services
{

    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        // Interner Cache der zuletzt geladenen Quests inkl. Goals
        private List<Quest> _cachedQuests = new();

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<Quest>();
            await _database.CreateTableAsync<Goal>();
        }

        public async Task<List<Quest>> GetQuestsWithGoalsAsync()
        {
            List<Quest> quests = await _database.Table<Quest>().OrderBy(q => q.Order).ToListAsync();

            foreach (Quest quest in quests)
            {
                quest.Goals = await _database.Table<Goal>()
                    .Where(g => g.QuestId == quest.Id)
                    .OrderBy(g => g.Order)
                    .ToListAsync();
            }

            _cachedQuests = CloneQuestListDeep(quests);
            return quests;
        }

        // Vergleicht die neue Quest-Liste mit dem Cache und speichert nur die Änderungen (Als Experiment, um MainViewModel schlank zu halten und da die Datenmenge gering sein sollte)
        public async Task SaveChangesIfNeededAsync(List<Quest> newQuests)
        {
            // Neue Quests identifizieren
            HashSet<int> newQuestIds = newQuests.Select(q => q.Id).ToHashSet();

            // Quests zum Löschen (im Cache, aber nicht mehr in newQuests)
            List<Quest> questsToDelete = _cachedQuests.Where(q => !newQuestIds.Contains(q.Id)).ToList();
            await DeleteQuestsWithGoalsAsync(questsToDelete);

            // Quests zum Einfügen und zum Updaten
            List<Quest> questsToInsert = newQuests.Where(q => q.Id == 0).ToList();
            await InsertQuestsWithGoalsAsync(questsToInsert);

            // Quests zum Updaten (im Cache, aber geändert in newQuests)
            List<Quest> questsToUpdate = newQuests.Where(q => q.Id != 0).Where(q =>
            {
                Quest? cached = _cachedQuests.FirstOrDefault(cq => cq.Id == q.Id);
                return cached != null && !q.Equals(cached);
            }).ToList();
            await UpdateQuestsWithGoalsAsync(questsToUpdate);

            // Cache aktualisieren (Kopie, damit keine Referenzen vermischt werden)
            _cachedQuests = CloneQuestListDeep(newQuests);
        }

        private async Task UpdateQuestsWithGoalsAsync(List<Quest> questsToUpdate)
        {
            foreach (Quest quest in questsToUpdate)
            {
                await _database.UpdateAsync(quest);

                // Goals vergleichen
                Quest? cachedQuest = _cachedQuests.FirstOrDefault(cq => cq.Id == quest.Id);
                List<Goal> cachedGoals = cachedQuest?.Goals ?? new List<Goal>();

                // Lösche Goals, die weg sind
                HashSet<int> newGoalIds = quest.Goals.Select(g => g.Id).ToHashSet();
                List<Goal> goalsToDelete = cachedGoals.Where(g => !newGoalIds.Contains(g.Id)).ToList();
                foreach (Goal goal in goalsToDelete)
                {
                    await _database.DeleteAsync(goal);
                }

                // Neue Goals einfügen
                List<Goal> goalsToInsert = quest.Goals.Where(g => g.Id == 0).ToList();
                foreach (Goal goal in goalsToInsert)
                {
                    goal.QuestId = quest.Id;
                    await _database.InsertAsync(goal);
                }

                // Geänderte Goals updaten
                List<Goal> goalsToUpdate = quest.Goals.Where(g => g.Id != 0).Where(g =>
                {
                    Goal? cachedGoal = cachedGoals.FirstOrDefault(cg => cg.Id == g.Id);
                    return cachedGoal != null && !g.Equals(cachedGoal);
                }).ToList();
                foreach (Goal goal in goalsToUpdate)
                {
                    await _database.UpdateAsync(goal);
                }
            }
        }

        private async Task InsertQuestsWithGoalsAsync(List<Quest> questsToInsert)
        {
            foreach (Quest quest in questsToInsert)
            {
                await _database.InsertAsync(quest);
                foreach (Goal goal in quest.Goals)
                {
                    goal.QuestId = quest.Id;
                    await _database.InsertAsync(goal);
                }
            }
        }

        private async Task DeleteQuestsWithGoalsAsync(List<Quest> questsToDelete)
        {
            foreach (Quest quest in questsToDelete)
            {
                // Alle Goals der Quest laden
                List<Goal> goalsToDelete = await _database.Table<Goal>()
                    .Where(g => g.QuestId == quest.Id)
                    .ToListAsync();

                foreach (Goal goal in goalsToDelete)
                {
                    await _database.DeleteAsync(goal);
                }

                await _database.DeleteAsync(quest);
            }
        }

        private List<Quest> CloneQuestListDeep(List<Quest> quests)
        {
            return quests.Select(q => new Quest
            {
                Id = q.Id,
                Title = q.Title,
                Color = q.Color,
                Order = q.Order,
                Goals = q.Goals.Select(g => new Goal
                {
                    Id = g.Id,
                    QuestId = g.QuestId,
                    Description = g.Description,
                    Order = g.Order,
                    IsCompleted = g.IsCompleted,
                    IsAddGoal = g.IsAddGoal
                }).ToList()
            }).ToList();
        }
    }
}
