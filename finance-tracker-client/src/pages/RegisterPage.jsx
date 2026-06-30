import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

export default function RegisterPage() {
    const navigate = useNavigate();
  
    const [form, setForm] = useState({
      firstName: '',
      lastName: '',
      email: '',
      password: ''
    });

    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
  
    const handleChange = (e) => {
      setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
    
        try {
          await axiosInstance.post('/auth/register', form);
          // Register başarılı → login sayfasına yönlendir
          navigate('/login', {
            state: { message: 'Kayıt başarılı! Giriş yapabilirsiniz.' }
          });
        } catch (err) {
          setError(err.response?.data?.message || 'Kayıt başarısız. Tekrar deneyin.');
        } finally {
          setLoading(false);
        }
      };

      return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center">
          <div className="bg-white p-8 rounded-xl shadow-md w-full max-w-md">
    
            {/* Başlık */}
            <div className="text-center mb-6">
              <h1 className="text-3xl font-bold text-blue-600">Finance Tracker 📈</h1>
              <p className="text-gray-500 mt-1">Yeni hesap oluşturun</p>
            </div>
    
            {/* Hata mesajı */}
            {error && (
              <div className="bg-red-100 text-red-600 px-4 py-2 rounded mb-4 text-sm">
                {error}
              </div>
            )}
    
            {/* Form */}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="flex gap-3">
                <div className="flex-1">
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Ad
                  </label>
                  <input
                    type="text"
                    name="firstName"
                    value={form.firstName}
                    onChange={handleChange}
                    required
                    className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="Adınız"
                  />
                </div>
                <div className="flex-1">
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Soyad
                  </label>
                  <input
                    type="text"
                    name="lastName"
                    value={form.lastName}
                    onChange={handleChange}
                    required
                    className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="Soyadınız"
                  />
                </div>
              </div>
    
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  E-mail
                </label>
                <input
                  type="email"
                  name="email"
                  value={form.email}
                  onChange={handleChange}
                  required
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="ornek@email.com"
                />
              </div>
    
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Şifre
                </label>
                <input
                  type="password"
                  name="password"
                  value={form.password}
                  onChange={handleChange}
                  required
                  minLength={6}
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  placeholder="En az 6 karakter"
                />
              </div>
    
              <button
                type="submit"
                disabled={loading}
                className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
              >
                {loading ? 'Kayıt yapılıyor...' : 'Kayıt Ol'}
              </button>
            </form>
    
            {/* Login linki */}
            <p className="text-center text-sm text-gray-500 mt-4">
              Zaten hesabınız var mı?{' '}
              <Link to="/login" className="text-blue-600 hover:underline">
                Giriş yapın
              </Link>
            </p>
          </div>
        </div>
      );
    }
    