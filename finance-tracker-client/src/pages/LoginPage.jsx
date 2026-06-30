import {useState} from 'react';
import {useNavigate, Link, useLocation} from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';
import { useAuth } from '../context/AuthContext';

export default function LoginPage(){
    const navigate = useNavigate();
    const {login} = useAuth();

    const location = useLocation();
    const successMessage = location.state?.message;

    const [form , setForm] = useState({email: '', password: ''});
    const[error, setError] = useState('');
    const[loading, setLoading] = useState(false);
    
    const handleChange= (e) => {
        setForm({...form, [e.target.name]: e.target.value});
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try{
            const response= await axiosInstance.post('/auth/login', form);
            const {token, firstName} = response.data;
            login(token,firstName);
            navigate('/dashboard');
        }
        catch(err){
            setError(err.response?.data?.message || 'Giriş başarısız. Tekrar deneyiniz.');
        }
        finally{
            setLoading(false);
        }
    };

    return (
        <div className='min-h-screen bg-gray-100 flex items-center justify-center'>
             <div className="bg-white p-8 rounded-xl shadow-md w-full max-w-md">
                 {/* Başlık */}
                <div className="text-center mb-6">
                <h1 className="text-3xl font-bold text-blue-600">Finance Tracker 📈</h1>
                <p className="text-gray-500 mt-1">Hesabınıza giriş yapın</p>
                </div>

                  {/* Başarı mesajı (register'dan gelince) */}
                {successMessage && (
                <div className="bg-green-100 text-green-600 px-4 py-2 rounded mb-4 text-sm">
                    {successMessage}
                </div>
                )}

                   {/* Hata mesajı */}
                {error && (
                <div className="bg-red-100 text-red-600 px-4 py-2 rounded mb-4 text-sm">
                    {error}
                </div>
                )}


                    {/* Form */}
        <form onSubmit={handleSubmit} className="space-y-4">
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
              className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="••••••"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
          >
            {loading ? 'Giriş yapılıyor...' : 'Giriş Yap'}
          </button>
        </form>

        
           {/* Register linki */}
           <p className="text-center text-sm text-gray-500 mt-4">
          Hesabınız yok mu?{' '}
          <Link to="/register" className="text-blue-600 hover:underline">
            Kayıt olun
          </Link>
        </p>
             </div>
        </div>
    );
};