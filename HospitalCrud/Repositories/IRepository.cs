namespace HospitalCrud.Repositories
{
	/// <summary>
	/// Generic interface for a CRUD data repository
	/// </summary>
	public interface IRepository<TEntity>
	{
		/// <summary>Add a new entity</summary>
		/// <param name="entity">The entity to be added</param>
		/// <returns>The asynchronous task to await for</returns>
		Task Add(TEntity entity);

		/// <summary>Update an existing entity</summary>
		/// <param name="entity">The entity to be updated</param>
		/// <returns>The asynchronous task to await for</returns>
		Task Update(TEntity entity);

		/// <summary>Delete an entity</summary>
		/// <param name="id">The id of the entity to be deleted</param>
		/// <returns>The asynchronous task to await for</returns>
		Task Delete(int? id);

		/// <summary>Retrieve an entity by its id</summary>
		/// <param name="id">The entity with the specified id</param>
		/// <returns>The asynchronous task to await for, which resolves to the entity object, if found</returns>
		Task<TEntity> GetById(int? id);

		/// <summary>Retrieve a collection containing all entities</summary>
		/// <returns>The asynchronous task to await for, which resolves to the collection of all existing entity objects</returns>
		Task<ICollection<TEntity>> GetAll();
	}
}
