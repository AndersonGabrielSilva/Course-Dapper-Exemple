using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using CursoDapperBalta.Dapper.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CursoDapperBalta
{

    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = @"Server=(localdb)\MSSQLLocalDB;DataBase=balta;Integrated Security=true;";

            //Exemplo AdoNet
            //new ExemploADONET(connectionString).ExecuteADONET();


            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connection Dapper");
                // ListCategories(connection);
                //  CreateCategories(connection);
                //  CreateManyCategories(connection);
                //  DeleteCategories(connection);
                // UpdateCategory(connection);
                // ExecuteProcedure(connection);
                // ExecuteReadProcedure(connection);
                // ExecuteScalar(connection);
                // ReadView(connection);
                // ListCategories(connection);
                // OneToOne(connection);
                //OneToMany(connection);
                // QueryMultple(connection);
                // SelectIn(connection);
                // Like(connection,"WebApi");
                Transaction(connection);

            }

            Console.WriteLine("Fim da Execulção");
            Console.ReadKey();
        }

        private static void DeleteCategories(SqlConnection connection)
        {
            var deleteQuery = "delete Category where id=@Id";

            var rows = connection.Execute(deleteQuery, new
            {
                Id = new Guid("dd6f7f05-9f98-4a14-a00d-f293daf345f3")
            });

            System.Console.WriteLine("Linhas deletadas: {0}", rows);
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = $@"UPDATE Category 
                                    set Title= @Title 
                                WHERE
                                    id=@id";

            var rows = connection.Execute(updateQuery, new
            {
                Id = new Guid("dd6f7f05-9f98-4a14-a00d-f293daf345f3"),
                Title = "Frontend 2035"
            });

            System.Console.WriteLine("Linhas atualizadas: {0}", rows);
        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("Select Id, Title from Category");

            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void CreateCategories(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Teste de Inserção";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Clound";
            category.Featured = false;

            var insertSql = $@"Insert into 
                                Category 
                            Values(
                                    @Id,
                                    @Title,
                                    @Url,
                                    @Description,
                                    @Order,
                                    @Summary,
                                    @Featured)";


            var rows = connection.Execute(insertSql, new
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Description = category.Description,
                Order = category.Order,
                Summary = category.Summary,
                Featured = category.Featured
            });

            Console.WriteLine("Linhas Inseridas : {0}", rows);
        }

        static void CreateManyCategories(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Teste de Inserção";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Clound";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Teste de Inserção";
            category2.Url = "amazon";
            category2.Description = "Categoria destinada a serviços do AWS";
            category2.Order = 8;
            category2.Summary = "AWS Clound";
            category2.Featured = false;



            var insertSql = $@"Insert into 
                                Category 
                            Values(
                                    @Id,
                                    @Title,
                                    @Url,
                                    @Description,
                                    @Order,
                                    @Summary,
                                    @Featured)";


            var rows = connection.Execute(insertSql, new[]
            {
                new {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Description = category.Description,
                Order = category.Order,
                Summary = category.Summary,
                Featured = category.Featured
                },
                 new {
                Id = category2.Id,
                Title = category2.Title,
                Url = category2.Url,
                Description = category2.Description,
                Order = category2.Order,
                Summary = category2.Summary,
                Featured = category2.Featured
                },
            });

            Console.WriteLine("Linhas Inseridas : {0}", rows);
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            //Nome da Store Procedure
            var procedure = "[spDeleteStudent]";

            var pars = new { StudentId = "" };

            var affectedRows = connection.Execute(
                procedure,
                pars,
                commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows}, linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            //Nome da Store Procedure
            var procedure = "[spGetCoursesByCategory]";

            var pars = new { CategoryId = "InformaOID" };

            var courses = connection.Query(
                procedure,
                pars,
                commandType: CommandType.StoredProcedure);

            foreach (var item in courses)
            {
                System.Console.WriteLine(item.Id);
            }
        }

        //Execulta uma ação no banco e retorna algo ao inves das linhas afetadas
        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Teste de Inserção";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Clound";
            category.Featured = false;

            var insertSql = $@"Insert into 
                                Category 
                            OUTPUT inserted.Id
                            Values(
                                    NEWID(),
                                    @Title,
                                    @Url,
                                    @Description,
                                    @Order,
                                    @Summary,
                                    @Featured)";


            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                Title = category.Title,
                Url = category.Url,
                Description = category.Description,
                Order = category.Order,
                Summary = category.Summary,
                Featured = category.Featured
            });

            Console.WriteLine("A categoria inserida foi : ", id);
        }

        //É exatamente como o Listar 
        static void ReadView(SqlConnection connection)
        {
            var viewQuery = "select*from _vwCousers";

            var cousers = connection.Query(viewQuery);

            foreach (var item in cousers)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        //Como Fazer o mapeamento Um pra Um
        static void OneToOne(SqlConnection connection)
        {
            var query = @"SELECT
                                *
                              FROM CareerItem 
                              INNER JOIN 
                                Course ON CareerItem.CourseID = Course.Id";

            //Informa no Carrer Item os dois objetos
            //Como Ler => Quero receber um CareerItem populado com um Couse e quero que me retorna um CareerItem
            //<T,S,R> => T = Objeto1, S = Objeto2, R=> o Que será retornado
            //Passo uma função anonima informando o que eu quero que ele faça, ou seja que pege o course e jogue para dentro do careerItem
            //No splitOn eu informo onde que quebra e comça o proximo objeto
            var items = connection.Query<CareerItem, Course, CareerItem>(
                query,
                    (careerItem, course) =>
                    {
                        careerItem.Course = course;
                        return careerItem;
                    }, splitOn: "Id"
                );

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - {item.Course.Title}");
            }
        }

        static void OneToMany(SqlConnection connection)
        {
            var query = @"SELECT 
                               [Career].[Id],
                               [Career].[Title],
                               [CareerItem].[CareerId] AS Id,
                               [CareerItem].[Title]
                           FROM
                               [Career]
                           INNER JOIN
                               [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]                           
                           ORDER BY 
                               [Career].[Title]";

            var careers = new List<Career>();

            var Items = connection.Query<Career, CareerItem, Career>(
                query,
                    (career, careerItem) =>//Para cada linha da minha consulta ele passa aqui dentro
                    {
                        var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();

                        if (car == null)
                        {
                            car = career;
                            car.Items.Add(careerItem);

                            careers.Add(car);
                        }
                        else
                        {
                            car.Items.Add(careerItem);
                        }

                        return career;
                    }, splitOn: "Id"
                );

            foreach (var career in Items)
            {
                Console.WriteLine($"{career.Title}");

                foreach (var item in career.Items)
                {
                    Console.WriteLine($"      {item.Title}");
                }
            }
        }

        //Maneira de realizar multiplas queries
        static void QueryMultple(SqlConnection connection)
        {

            var query = "SELECT * FROM Category; SELECT * FROM Course;";

            using (var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>();
                var courses = multi.Read<Course>();


                Console.WriteLine("---------------------Categorias------------------------");
                Console.WriteLine();
                foreach (var item in categories)
                {
                    Console.WriteLine($"{item.Title}");
                }

                Console.WriteLine();
                Console.WriteLine("-----------------------Cursos-------------------------");
                Console.WriteLine();
                foreach (var item in courses)
                {
                    Console.WriteLine($"{item.Title}");
                }

            }
        }

        static void SelectIn(SqlConnection connection)
        {
            var query = @"SELECT * FROM Career WHERE Id IN @Id";

            var careers = connection.Query<Career>(query, new
            {
                Id = new[]
                {
                    "e6730d1c-6870-4df3-ae68-438624e04c72",
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b"
                }
            });

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");

            }
        }

        static void Like(SqlConnection connection, string termo)
        {
            var query = @"SELECT * FROM Course WHERE Title LIKE @expressao";

            var cousers = connection.Query<Career>(query, new
            {
                expressao = $"%{termo}%"
            });

            foreach (var course in cousers)
            {
                Console.WriteLine($"{course.Title}");

            }
        }

        static void Transaction(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Não quero Incluir";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Clound";
            category.Featured = false;

            var insertSql = $@"Insert into 
                                Category 
                            Values(
                                    @Id,
                                    @Title,
                                    @Url,
                                    @Description,
                                    @Order,
                                    @Summary,
                                    @Featured)";

            connection.Open();
            //TUdo realizado dentro deste transaction está dentro uma transação
            using (var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insertSql, new
                {
                    Id = category.Id,
                    Title = category.Title,
                    Url = category.Url,
                    Description = category.Description,
                    Order = category.Order,
                    Summary = category.Summary,
                    Featured = category.Featured
                },transaction);

                // transaction.Commit();
                transaction.Rollback();

                Console.WriteLine("Linhas Inseridas : {0}", rows);
            }

        }

    }
}
