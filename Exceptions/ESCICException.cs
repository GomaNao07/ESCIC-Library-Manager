using System;

namespace ESCICLibraryManager.Exceptions
{
	public class ESCICException : Exception
	{
		public ESCICException(string message) : base(message)
		{
		}

		public ESCICException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class LivreException : ESCICException
	{
		public LivreException(string message) : base(message)
		{
		}

		public LivreException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UtilisateurException : ESCICException
	{
		public UtilisateurException(string message) : base(message)
		{
		}

		public UtilisateurException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class EmpruntException : ESCICException
	{
		public EmpruntException(string message) : base(message)
		{
		}

		public EmpruntException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class DatabaseException : ESCICException
	{
		public DatabaseException(string message) : base(message)
		{
		}

		public DatabaseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class ValidationException : ESCICException
	{
		public ValidationException(string message) : base(message)
		{
		}
	}
}