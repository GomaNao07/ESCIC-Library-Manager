using System;

namespace ESCIC_Library_Manager.Exceptions
{
    public class ESCICException : Exception
    {
        public ESCICException(string message) : base(message) { }
    }

    public class LivreException : ESCICException 
    { 
        public LivreException(string m) : base(m) { } 
    }

    public class UtilisateurException : ESCICException 
    { 
        public UtilisateurException(string m) : base(m) { } 
    }

    public class EmpruntException : ESCICException 
    { 
        public EmpruntException(string m) : base(m) { } 
    }

    public class DatabaseException : ESCICException 
    { 
        public DatabaseException(string m) : base(m) { } 
    }

    public class ValidationException : ESCICException 
    { 
        public ValidationException(string m) : base(m) { } 
    }
}
