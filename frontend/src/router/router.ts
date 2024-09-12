import { RouteRecordRaw } from 'vue-router'
import { createRouter } from 'vue-router'
import { createWebHistory } from 'vue-router'
import IndexView from '../components/views/IndexView.vue'
import RegisterView from '../components/views/RegisterView.vue'
import LoginView from '../components/views/LoginView.vue'
import DashboardView from '../components/views/DashboardView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Index',
    component: IndexView,
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
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: DashboardView,
    meta: {
      requiresAuth : true,
    }
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

router.beforeEach((to, _from, next) => {
  if (!to.meta.requiresAuth) {
    next();
  }

  const token = localStorage.getItem('jwt_token');

  if (!token) {
    next('/login');
  }

  next();
});

export default router;