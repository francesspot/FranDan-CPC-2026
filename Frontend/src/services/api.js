const API_BASE_URL = "https://localhost:7074";

const getHeaders = () => {
  const token = localStorage.getItem("token");
  return {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };
};

export const api = {
  auth: {
    login: async (credentials) =>
      fetch(`${API_BASE_URL}/api/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials),
      }),
    register: async (userData) =>
      fetch(`${API_BASE_URL}/api/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData),
      }),
    verify: async (data) =>
      fetch(`${API_BASE_URL}/api/auth/verify`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      }),
  },

  user: {
    getFull: async () =>
      fetch(`${API_BASE_URL}/api/user/full`, {
        method: "GET",
        headers: getHeaders(), // Wykorzystujemy Twoją funkcję!
      }),

    update: async (data) =>
      fetch(`${API_BASE_URL}/api/user/edit`, {
        method: "PUT",
        headers: getHeaders(),
        body: JSON.stringify(data),
      }),
    delete: async () =>
      fetch(`${API_BASE_URL}/api/user/delete`, {
        method: "DELETE",
        headers: getHeaders(),
      }),
  },

  friends: {
    invite: async (usernameOrEmail) =>
      fetch(`${API_BASE_URL}/api/friend/invite`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify({ usernameOrEmail }),
      }),
    accept: async (id) =>
      fetch(`${API_BASE_URL}/api/friend/accept?request=${id}`, {
        method: "PATCH",
        headers: getHeaders(),
      }),
    reject: async (id) =>
      fetch(`${API_BASE_URL}/api/friend/reject?request=${id}`, {
        method: "PATCH",
        headers: getHeaders(),
      }),
    delete: async (id) =>
      fetch(`${API_BASE_URL}/api/friend/delete?request=${id}`, {
        method: "DELETE",
        headers: getHeaders(),
      }),
  },

  plans: {
    create: async (planData) =>
      fetch(`${API_BASE_URL}/api/plan/create`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(planData),
      }),
    getFull: async (planId) =>
      fetch(`${API_BASE_URL}/api/plan/full?request=${planId}`, {
        method: "GET",
        headers: getHeaders(),
      }),
    edit: async (planData) =>
      fetch(`${API_BASE_URL}/api/plan/edit`, {
        method: "PUT",
        headers: getHeaders(),
        body: JSON.stringify(planData),
      }),
    delete: async (planId) =>
      fetch(`${API_BASE_URL}/api/plan/delete?request=${planId}`, {
        method: "DELETE",
        headers: getHeaders(),
      }),
  },

  participation: {
    add: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/add`, {
        method: "POST",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),

    remove: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/delete`, {
        method: "DELETE",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),

    accept: async (
      payload, // oczekuje np. { id: 5 }
    ) =>
      fetch(`${API_BASE_URL}/api/participation/accept`, {
        method: "PATCH",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),

    reject: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/reject`, {
        method: "PATCH",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),

    setAdmin: async (payload) =>
      fetch(`${API_BASE_URL}/api/participation/set-admin`, {
        method: "PATCH",
        headers: getHeaders(),
        body: JSON.stringify(payload),
      }),
  },
};
