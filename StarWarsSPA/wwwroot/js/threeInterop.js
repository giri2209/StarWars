window.initializeThreeJS = () => {

    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth, window.innerHeight);
    document.getElementById("three-canvas").appendChild(renderer.domElement);


    const starsGeometry = new THREE.SphereGeometry(100, 32, 32);
    const starsMaterial = new THREE.PointsMaterial({ color: 0x888888, size: 0.7 });
    const stars = new THREE.Points(starsGeometry, starsMaterial);
    scene.add(stars);


    const animate = function () {
        requestAnimationFrame(animate);


        stars.rotation.z += 0.0005;


        renderer.render(scene, camera);
    };

    animate();
};