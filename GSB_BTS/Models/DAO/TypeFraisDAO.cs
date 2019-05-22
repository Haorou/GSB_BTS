using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models.DAO
{
    public class TypeFraisDAO : DAO_Manager
    {
        public List<TypeFrais> ReadAll()
        {
            List<TypeFrais> List_TypeFrais = new List<TypeFrais>();
            TypeFrais typeFrais;

            if (OpenConnection())
            {

                TypeFraisDAO typeFraisManager = new TypeFraisDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                        "FROM type_frais";

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    typeFrais = (TypeFrais.Frais)Enum.Parse(typeof(TypeFrais.Frais), (string)dataReader["type"]);

                    List_TypeFrais.Add(typeFrais);

                }

                dataReader.Close();
                CloseConnection();
            }
            return List_TypeFrais;
        }
    }
}