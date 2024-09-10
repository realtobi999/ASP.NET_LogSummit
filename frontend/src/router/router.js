import { createWebHistory, createRouter } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'Index',
    component: () => import('../views/Index.vue'),
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../views/Register.vue'),
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
