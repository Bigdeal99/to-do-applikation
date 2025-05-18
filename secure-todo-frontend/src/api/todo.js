import api from './axios';

export const getTodos = async (filters = {}) => {
  const params = new URLSearchParams(filters).toString();
  const res = await api.get(`/Todo${params ? `?${params}` : ''}`);
  return res.data;
};

export const createTodo = async (todo) => {
  const res = await api.post('/Todo', todo);
  return res.data;
};

export const updateTodo = async (id, todo) => {
  await api.put(`/Todo/${id}`, todo);
};

export const deleteTodo = async (id) => {
  await api.delete(`/Todo/${id}`);
};
