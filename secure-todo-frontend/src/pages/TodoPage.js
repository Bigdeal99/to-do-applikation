import React, { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../auth/AuthContext';
import { getTodos, createTodo, updateTodo, deleteTodo } from '../api/todo';
import { useNavigate } from 'react-router-dom';

const defaultForm = {
  id: null,
  title: '',
  description: '',
  category: '',
  dueDate: '',
};

const formatDate = (dateString) => {
  if (!dateString) return '';
  return dateString.split('T')[0];
};

const TodoPage = () => {
  const { logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const [todos, setTodos] = useState([]);
  const [newTitle, setNewTitle] = useState('');
  const [newCategory, setNewCategory] = useState('');
  const [newDueDate, setNewDueDate] = useState('');
  const [error, setError] = useState('');
  const [filter, setFilter] = useState({ category: '', isCompleted: '' });

  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState(defaultForm);

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
            <button onClick={() => {
              setFormData({
                id: todo.id,
                title: todo.title,
                description: todo.description || '',
                category: todo.category || '',
                dueDate: formatDate(todo.dueDate),
              });
              setShowModal(true);
            }}>Edit</button>
          </li>
        ))}
      </ul>

      {showModal && (
        <div style={{
          position: 'fixed', top: 0, left: 0, right: 0, bottom: 0,
          background: 'rgba(0,0,0,0.5)', display: 'flex',
          justifyContent: 'center', alignItems: 'center'
        }}>
          <div style={{ background: 'white', padding: 20, width: '400px' }}>
            <h3>Edit Todo</h3>
            <input
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              placeholder="Title"
            /><br />
            <textarea
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Description"
            /><br />
            <input
              value={formData.category}
              onChange={(e) => setFormData({ ...formData, category: e.target.value })}
              placeholder="Category"
            /><br />
            <input
              type="date"
              value={formatDate(formData.dueDate)}
              onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
            /><br />
            <button onClick={() => setShowModal(false)}>Cancel</button>
            <button onClick={async () => {
              try {
                await updateTodo(formData.id, formData);
                setTodos(todos.map(t => (t.id === formData.id ? { ...t, ...formData } : t)));
                setShowModal(false);
              } catch {
                setError('Failed to update todo');
              }
            }}>Save</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default TodoPage;
