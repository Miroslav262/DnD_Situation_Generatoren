using dndsitgen.Models;
using Npgsql;
using Dapper;
using BCrypt.Net;

namespace dndsitgen.Repository
{
    public class UserRepository
    {
        private readonly string con_str;

        public UserRepository(string con_str)
        {
            this.con_str = con_str;
        }

        public async Task<UserModel?> getByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<UserModel>(
                """
                select id, name, pass_hash 
                from "users"
                where id = @id
                """,
                new { id }
            );
        }

        public async Task<int?> getIdByNameAsync(string name)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<int?>(
                """
                select id 
                from "users"
                where name = @name
                """,
                new { name }
            );
        }

        public async Task<UserModel?> createUser(UserModel user)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            var existing = await connection.QueryFirstOrDefaultAsync<UserModel>(
                """
                select id, name, pass_hash 
                from "users"
                where name = @name
                """,
                new { name = user.name }
            );

            if (existing != null)
                return null;

            string pass_hash = BCrypt.Net.BCrypt.HashPassword(user.pass_hash);

            int newId = await connection.ExecuteScalarAsync<int>(
                """
                insert into "users"(name, pass_hash)
                values(@name, @pass_hash)
                returning id
                """,
                new { name = user.name, pass_hash }
            );

            return new UserModel
            {
                id = newId,
                name = user.name,
                pass_hash = pass_hash
            };
        }

        public async Task<bool?> checkUser(UserModel user)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            string? pass_hash = await connection.QueryFirstOrDefaultAsync<string>(
                """
                select pass_hash 
                from "users"
                where name = @name
                """,
                new { name = user.name }
            );

            if (pass_hash == null)
                return null;

            return BCrypt.Net.BCrypt.Verify(user.pass_hash, pass_hash);
        }
    }
}
