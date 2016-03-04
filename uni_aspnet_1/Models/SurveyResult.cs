using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uni_aspnet_1.Models
{
    public enum HobbyType { 
        Student, Worker, Undefined
    };

    public class SurveyResult{

        public string name, surname;
        public HobbyType hobby;

        public SurveyResult(string _name, string _surname, HobbyType _hobby) {
            name = _name;
            surname = _surname;
            hobby = _hobby;
        }

        public string localizedHobby() {
            switch (hobby) { 
                case HobbyType.Student:
                    return "Студент";
                case HobbyType.Worker:
                    return "Рабочий";                
            }

            return "Не указано";
        }
        
    }
}