import React, { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../auth/AuthContext';
import { getTodos, createTodo, updateTodo, deleteTodo } from '../api/todo';
import { useNavigate } from 'react-router-dom';

const TodoPage = () => {
  const { logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const [todos, setTodos] = useState([]);
  const [newTitle, setNewTitle] = useState('');
  const [newCategory, setNewCategory] = useState('');
  const [newDueDate, setNewDueDate] = useState('');
  const [error, setError] = useState('');
  const [filter, setFilter] = useState({ category: '', isCompleted: '' });

  const loadTodos = async () => {
    try {
      const data = await getTodos(filter);
      setTodos(data);
    } catch (err) {
      if (err.response?.status === 401) {
        logout();
        navigate('/login');
      } else {
        setError('Failed to load todos');
      }
    }
  };

  useEffect(() => {
    loadTodos();
  }, [filter]);

  const handleAddTodo = async () => {
    try {
      const todo = {
        title: newTitle,
        category: newCategory || null,
        dueDate: newDueDate || null,
      };
      const added = await createTodo(todo);
      setTodos([...todos, added]);
      setNewTitle('');
      setNewCategory('');
      setNewDueDate('');
    } catch {
      setError('Failed to add todo');
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteTodo(id);
      setTodos(todos.filter(t => t.id !== id));
    } catch {
      setError('Failed to delete');
    }
  };

  const handleToggleComplete = async (todo) => {
    try {
      const updated = { ...todo, isCompleted: !todo.isCompleted };
      await updateTodo(todo.id, updated);
      setTodos(todos.map(t => (t.id === todo.id ? updated : t)));
    } catch {
      setError('Failed to update');
    }
  };

  return (
    <div>
      <h2>My To-Do List</h2>
      <button onClick={logout}>Logout</button>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      <div>
        <input
          placeholder="New task"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />
        <input
          placeholder="Category (optional)"
          value={newCategory}
          onChange={(e) => setNewCategory(e.target.value)}
        />
        <input
          type="date"
          value={newDueDate}
          onChange={(e) => setNewDueDate(e.target.value)}
        />
        <button onClick={handleAddTodo}>Add</button>
      </div>

      <div style={{ marginTop: '1em' }}>
        <label>Filter by category:</label>
        <input
          type="text"
          value={filter.category}
          onChange={(e) => setFilter({ ...filter, category: e.target.value })}
        />
        <label>Completed:</label>
        <select
          value={filter.isCompleted}
          onChange={(e) => setFilter({ ...filter, isCompleted: e.target.value })}
        >
          <option value="">All</option>
          <option value="true">✔ Done</option>
          <option value="false">✘ Not Done</option>
        </select>
      </div>

      <ul style={{ marginTop: '1em' }}>
        {todos.map((todo) => (
          <li key={todo.id}>
            <span style={{ textDecoration: todo.isCompleted ? 'line-through' : 'none' }}>
              {todo.title} ({todo.category || 'No category'}) - {todo.dueDate?.split('T')[0] || 'No due date'}
            </span>
            <button onClick={() => handleToggleComplete(todo)}>
              {todo.isCompleted ? 'Undo' : 'Complete'}
            </button>
            <button onClick={() => handleDelete(todo.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TodoPage;
