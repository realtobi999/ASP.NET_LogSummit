import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import IndexView from '../components/views/IndexView.vue'
import RegisterView from '../components/views/RegisterView.vue'
import LoginView from '../components/views/LoginView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Index',
    component: IndexView
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterView
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginView
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

export default router
