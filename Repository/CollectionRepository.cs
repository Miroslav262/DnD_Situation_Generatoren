using dndsitgen.Models;
using Npgsql;
using Dapper;

namespace dndsitgen.Repository
{
    
    public class CollectionRepository
    {
        private readonly string con_str;


        public CollectionRepository(string con_str) {
            this.con_str = con_str;
        }

        public async Task<BattleScene[]?> getBattleScenesByUserIdAsync(int userId)
        {
            await using NpgsqlConnection conection = new NpgsqlConnection(con_str);
            try
            {
                await conection.OpenAsync();
                return (await conection.QueryAsync<BattleScene>("""
                    select bs.id, bs.name, bs.description
                    from "battle_scene" bs
                    inner join "user_battle_scene_ratio" ubsr on ubsr.battle_scene_id=bs.id
                    where ubsr.user_id=@user_id;
                    """, new { user_id=userId})).
                   ToArray<BattleScene>();
                
            }
            catch {
                return null;
            }
        }
        public async Task<bool> createBattleScene(BattleScene battleScene, int userId) {
            await using NpgsqlConnection connection = new NpgsqlConnection(con_str);
            NpgsqlTransaction? transaction = null;
            try {

                await connection.OpenAsync();

                transaction = await connection.BeginTransactionAsync();

                int? id = await connection.ExecuteScalarAsync<int>("""
                    insert into "battle_scene"(name, description)
                    values(@name, @description)
                    returning id;
                    """, new { name=battleScene.Name, description=battleScene.Description}, transaction);
                if (id != null)
                {
                    await connection.ExecuteAsync("""
                        insert into "user_battle_scene_ratio" (user_id, battle_scene_id)
                        values(@user_id, @battle_scene_id);
                        """, new { user_id = userId, battle_scene_id = id }, transaction);

                    await transaction.CommitAsync();

                    return true;
                }
                else {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch {
                if (transaction != null) { await transaction.RollbackAsync(); }
                return false;
            }
        }

        public async Task<bool> updateBattleScene(BattleScene battleScene)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                int affected = await connection.ExecuteAsync(
                    """
            update "battle_scene"
            set name = @name,
                description = @description
            where id = @id;
            """,
                    new
                    {
                        id = battleScene.Id,
                        name = battleScene.Name,
                        description = battleScene.Description
                    },
                    transaction
                );

                await transaction.CommitAsync();

                return affected > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> deleteBattleScene(int id)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await connection.ExecuteAsync(
                    """
            delete from "battle_scene_creature_ratio"
            where battle_scene_id = @id;
            """,
                    new { id },
                    transaction
                );

                await connection.ExecuteAsync(
                    """
            delete from "user_battle_scene_ratio"
            where battle_scene_id = @id;
            """,
                    new { id },
                    transaction
                );

                int affected = await connection.ExecuteAsync(
                    """
            delete from "battle_scene"
            where id = @id;
            """,
                    new { id },
                    transaction
                );

                await transaction.CommitAsync();

                return affected > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


    }
}
